resource "aws_ecs_cluster" "this" {
  name = "${var.prefix}-${var.environment}"
}

resource "aws_security_group" "trip-analyzer" {
  name        = "${var.prefix}-trip-analyzer-${var.environment}"
  description = "Fargate trip-analyzer"
  vpc_id      = var.vpc_id

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }
  tags = var.tags
}

resource "aws_ecs_service" "this" {
  name             = "${var.prefix}-${var.environment}"
  cluster          = aws_ecs_cluster.this.id
  task_definition  = aws_ecs_task_definition.this.arn
  desired_count    = var.desired_count
  launch_type      = "FARGATE"
  platform_version = "1.4.0"
  deployment_minimum_healthy_percent = 50
  deployment_maximum_percent         = 200
  network_configuration {
    security_groups = [ var.security_group_vpc,
                        var.aws_security_group_alb_id,
                        aws_security_group.trip-analyzer.id]
    subnets         = var.private_subnets
    assign_public_ip = false
  }

  load_balancer {
    target_group_arn = var.aws_alb_target_group_arn
    container_name   = "trip-analyzer"
    container_port   = 80
  }

}

resource "aws_ecs_task_definition" "this" {
  family                   = "${var.prefix}-${var.environment}"
  execution_role_arn       = aws_iam_role.task_execution_role.arn
  task_role_arn            = aws_iam_role.task_execution_role.arn
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = var.task_cpu
  memory                   = var.task_memory
  container_definitions = jsonencode([{
      name  = "trip-analyzer"
      image = join("@", [var.aws_ecr_repository_url, var.aws_ecr_image_digest])
      portMappings = [
        {
          containerPort: 80,
          protocol: "tcp",
          hostPort: 80
        }
    ]
    }])
}

resource "aws_appautoscaling_target" "instance" {
  max_capacity       = 5
  min_capacity       = 1
  resource_id        = "service/${aws_ecs_cluster.this.name}/${aws_ecs_service.this.name}"
  service_namespace  = "ecs"
  scalable_dimension = "ecs:service:DesiredCount"
}

resource "aws_appautoscaling_policy" "instance" {
  name               = "${var.prefix}-${var.environment}-ecs-cpu-auto-scaling"
  policy_type        = "TargetTrackingScaling"
  service_namespace  = aws_appautoscaling_target.instance.service_namespace
  scalable_dimension = aws_appautoscaling_target.instance.scalable_dimension
  resource_id        = aws_appautoscaling_target.instance.resource_id

  target_tracking_scaling_policy_configuration {
    predefined_metric_specification {
      predefined_metric_type = "ECSServiceAverageCPUUtilization"
    }

    target_value       = 80
    scale_in_cooldown  = 300
    scale_out_cooldown = 300
  }
}