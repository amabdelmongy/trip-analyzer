variable "environment" {
  description = "Name of the application environment. e.g. dev, prod, test, staging"
}

variable "prefix" {
  description = "Prefix for all the resources to be created. Please note thst 2 allows only lowercase alphanumeric characters and hyphen"
}

variable "tags" {
  description = "tags"
}

variable "desired_count" {
  description = "The number of instances of fargate tasks to keep running"
  default     = "2"
}

variable "scaling_up_cooldown" {
  description = "The amount of time, in seconds, after a scaling activity completes and before the next scaling activity can start (upscaling)"
  default     = "60"
}

variable "scaling_up_adjustment" {
  description = " The number of tasks by which to scale, when the upscaling parameters are breached"
  default     = "1"
}

variable "scaling_down_cooldown" {
  description = "The amount of time, in seconds, after a scaling activity completes and before the next scaling activity can start (downscaling)"
  default     = "300"
}

variable "scaling_down_adjustment" {
  description = " The number of tasks by which to scale (negative for downscaling), when the downscaling parameters are breached"
  default     = "-1"
}

variable "task_memory" {
  description = "The amount (in MiB) of memory used by the task"
  default     = 2048
}

variable "task_cpu" {
  description = "The number of cpu units used by the task"
  default     = 1024
}

variable "private_subnets" {
  description = "private subnets ids"
}

variable "public_subnets" {
  description = "public subnets ids"
}

variable "vpc_id" {
  description = "vpc id"
}

variable "security_group_vpc" {
  description = "security group for vpc"
}

variable "aws_alb_target_group_arn" {
  description = "target group pf the alb"
}

variable "aws_security_group_alb_id" {
  description = "id of security group of the alb"
}

variable "aws_ecr_repository_url" {
  description = "aws ecr repository url"
}
variable "aws_ecr_image_digest" {
  description = "aws ecr image digest"
}
