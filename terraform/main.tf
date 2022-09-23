module "vpc" {
  source = "./vpc"
  tags                                 = var.tags
  prefix                               = var.prefix
  environment                          = var.environment
}

module "ecs" {
  source                               = "./ecs"
  vpc_id                               = module.vpc.vpc_id
  private_subnets                      = module.vpc.private_subnets
  public_subnets                       = module.vpc.public_subnets
  security_group_vpc                               = module.vpc.security_group_vpc
  aws_alb_target_group_arn             = module.alb.aws_alb_target_group_arn
  aws_security_group_alb_id            = module.alb.aws_security_group_alb_id
  aws_ecr_repository_url               = aws_ecr_repository.instance.repository_url
  aws_ecr_image_digest                 = aws_ecr_repository.instance.repository_url
  tags                                 = var.tags
  prefix                               = var.prefix
  environment                          = var.environment
}

module "alb" {
  depends_on = [
    module.vpc.vpc_id
  ]
  source                              = "./alb"
  public_subnets                      = module.vpc.public_subnets
  vpc_id                              = module.vpc.vpc_id
  tags                                = var.tags
}

output "ecr_repository_name" {
  value = aws_ecr_repository.instance.name
}