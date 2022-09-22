data "aws_availability_zones" "available" {
  state = "available"
}

module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  name = "sample_vpc"
  cidr = "10.0.0.0/16"

  azs             = data.aws_availability_zones.available.names
  private_subnets = ["10.0.1.0/24", "10.0.2.0/24"]
  enable_ipv6          = false 
  enable_dns_hostnames = true
  enable_dns_support   = true

  tags = {
    Name      = "basic-example-vpc"
    Terraform = "true"
  }
}
resource "aws_lb" "instance" {
  name               = "alb"
  load_balancer_type = "application"
  subnets = module.vpc.private_subnets
}

resource "aws_lb_listener" "instance" {
  load_balancer_arn = aws_lb.instance.arn
  port              = 80
  protocol          = "HTTP"
  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.instance.arn
  }
}

resource "aws_lb_target_group" "instance" {
  name                 = "alb-target-group"
  target_type          = "ip"
  protocol             = "HTTP"
  port                 = 8400
  vpc_id               = module.vpc.vpc_id
  deregistration_delay = 30 // seconds
  health_check {
    interval          = 5 // seconds
    timeout           = 2 // seconds
    healthy_threshold = 2
    protocol          = "HTTP"
    path              = "/health"
  }
}
