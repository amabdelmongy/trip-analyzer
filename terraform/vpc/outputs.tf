output "public_subnets" {
  value = module.vpc.public_subnets
}

output "private_subnets" {
  value = module.vpc.private_subnets
}

output "vpc_id" {
  value = module.vpc.vpc_id
}

output "security_group_vpc" {
  value = aws_security_group.security_group_vpc.id
}

output "azs" {
  value = module.vpc.azs
}