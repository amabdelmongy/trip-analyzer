resource "aws_ecr_repository" "instance" {
  name = "trip-analyzer-dev-ecr-repository"
  tags = var.tags
}

data "aws_ecr_repository" "instance" {
  name = aws_ecr_repository.instance.name
}

data "aws_ecr_image" "instance" {
  repository_name = aws_ecr_repository.instance.name
  image_tag       = "trip-analyzer-dev-ecr-image"
}