data "aws_iam_policy_document" "assume_role_policy_document" {
  version = "2012-10-17"
  statement {
    effect  = "Allow"
    actions = ["sts:AssumeRole"]
    principals {
      identifiers = ["ecs-tasks.amazonaws.com"]
      type        = "Service"
    }
  }
}

resource "aws_iam_role" "task_execution_role" {
  name               = "ecs-execution-role"
  assume_role_policy = data.aws_iam_policy_document.assume_role_policy_document.json
}

resource "aws_iam_role_policy_attachment" "instance" {
  role       = aws_iam_role.task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

#   container_definitions    = <<CONTAINER_DEFINITION
# [
#   {
#     "essential": true,
#     "image": "trip-analyzer",
#     "name": "trip-analyzer",
#     "portMappings": [
#       {
#         "containerPort": 80,
#         "protocol": "tcp",
#         "hostPort": 80
#       }
#     ]
#   }
# ]
# CONTAINER_DEFINITION

# resource "aws_iam_role" "task_execution_role" {
#   name = "${var.prefix}-task-execution-role-${var.environment}"
#   tags = var.tags

#   assume_role_policy = <<EOF
# {
#   "Version": "2012-10-17",
#   "Statement": [
#     {
#       "Effect": "Allow",
#       "Principal": {
#         "Service": [
#           "ecs-tasks.amazonaws.com"
#         ]
#       },
#       "Action": "sts:AssumeRole"
#     }
#   ]
# }
# EOF
# }

# resource "aws_iam_policy" "task_execution_policy" {
#   policy = <<EOF
# {
#   "Version": "2012-10-17",
#   "Statement": [
#     {
#       "Effect": "Allow",
#       "Action": [
#         "ecr:GetAuthorizationToken",
#         "ecr:BatchCheckLayerAvailability",
#         "ecr:GetDownloadUrlForLayer",
#         "ecr:BatchGetImage",
#         "logs:CreateLogStream",
#         "logs:PutLogEvents",
#         "ssm:GetParameters",
#         "kms:Decrypt"
#       ],
#       "Resource": "*"
#     }
#   ]
#   }
# EOF
# }

# resource "aws_iam_role_policy_attachment" "task_execution_policy_attach" {
#   role       = aws_iam_role.task_execution_role.name
#   policy_arn = aws_iam_policy.task_execution_policy.arn
# }

# resource "aws_iam_role" "task_role" {
#   name = "${var.prefix}-task-role-${var.environment}"
#   tags = var.tags

#   assume_role_policy = <<EOF
# {
#   "Version": "2012-10-17",
#   "Statement": [
#     {
#       "Effect": "Allow",
#       "Principal": {
#         "Service": [
#           "ecs-tasks.amazonaws.com"
#         ]
#       },
#       "Action": "sts:AssumeRole"
#     }
#   ]
# }
# EOF
# }