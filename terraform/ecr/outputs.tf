output "aws_ecr_repository_url" {
  value = aws_ecr_repository.instance.repository_url
}

output "aws_ecr_image_digest" {
  value = data.aws_ecr_image.instance.image_digest
}

output "ecr_repository_name" {
  value = aws_ecr_repository.instance.name
}