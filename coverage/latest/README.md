# Summary
<details open><summary>Summary</summary>

|||
|:---|:---|
| Generated on: | 07/19/2026 - 19:27:35 |
| Coverage date: | 07/19/2026 - 19:24:18 |
| Parser: | Cobertura |
| Assemblies: | 3 |
| Classes: | 57 |
| Files: | 57 |
| **Line coverage:** | 97.4% (863 of 886) |
| Covered lines: | 863 |
| Uncovered lines: | 23 |
| Coverable lines: | 886 |
| Total lines: | 1886 |
| **Branch coverage:** | 95.5% (65 of 68) |
| Covered branches: | 65 |
| Total branches: | 68 |
| **Method coverage:** | [Feature is only available for sponsors](https://reportgenerator.io/pro) |

</details>

## Coverage
<details><summary>Blogging.Api - 98.1%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Blogging.Api**|**98.1%**|**100%**|
|Blogging.Api.Contracts.ApiErrorResponse|100%||
|Blogging.Api.DependencyInjection.AddBloggingApiExtension|91.3%||
|Blogging.Api.DependencyInjection.AddBloggingDomainExtension|100%||
|Blogging.Api.DependencyInjection.UseBloggingApiAsyncExtension|100%||
|Blogging.Api.DependencyInjection.UseBloggingAsyncExtension|100%||
|Blogging.Api.Endpoints.MapBloggingEndpointsExtension|100%||
|Blogging.Api.Endpoints.Posts.MapCreateCommentEndpointExtension|100%||
|Blogging.Api.Endpoints.Posts.MapCreatePostEndpointExtension|100%||
|Blogging.Api.Endpoints.Posts.MapGetPostEndpointExtension|89.7%||
|Blogging.Api.Endpoints.Posts.MapListPostsEndpointExtension|100%||
|Blogging.Api.Endpoints.Posts.MapPostsEndpointsExtension|100%||
|Blogging.Api.Endpoints.Posts.MapSearchPostsEndpointExtension|100%||
|Blogging.Api.ErrorHandling.GlobalExceptionHandler|100%|100%|
|Blogging.Api.Posts.Contracts.CommentResponse|100%||
|Blogging.Api.Posts.Contracts.CreateCommentRequest|100%||
|Blogging.Api.Posts.Contracts.CreatePostRequest|100%||
|Blogging.Api.Posts.Contracts.PagedPostSearchResponse|100%||
|Blogging.Api.Posts.Contracts.PostDetailResponse|100%||
|Blogging.Api.Posts.Contracts.PostListItemResponse|100%||
|Blogging.Api.Posts.Contracts.PostSearchRequest|100%||
|Program|100%||

</details>
<details><summary>Blogging.Domain - 97.5%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Blogging.Domain**|**97.5%**|**97.8%**|
|Blogging.Domain.DependencyInjection.AddBloggingDomainExtension|100%||
|Blogging.Domain.Entities.BlogPost|100%||
|Blogging.Domain.Entities.Comment|75%||
|Blogging.Domain.Posts.BlogPostDetail|100%||
|Blogging.Domain.Posts.BlogPostService|100%||
|Blogging.Domain.Posts.BlogPostSummary|100%||
|Blogging.Domain.Posts.CommentSummary|100%||
|Blogging.Domain.Posts.CreateBlogPostCommand|100%||
|Blogging.Domain.Posts.CreateCommentCommand|100%||
|Blogging.Domain.Posts.PagedResult`1|100%||
|Blogging.Domain.Posts.PostFilter|100%||
|Blogging.Domain.Posts.PostSearchService|100%|100%|
|Blogging.Domain.Posts.PostSort|100%||
|Blogging.Domain.Posts.PostSpecifications|100%|100%|
|Blogging.Domain.Posts.Specifications.AllPostsSpecification|100%||
|Blogging.Domain.Posts.Specifications.HasCommentsSpecification|100%||
|Blogging.Domain.Posts.Specifications.MaximumCommentCountSpecification|100%||
|Blogging.Domain.Posts.Specifications.MinimumCommentCountSpecification|100%||
|Blogging.Domain.Posts.Specifications.PostContentSpecification|100%||
|Blogging.Domain.Posts.Specifications.PostIdSpecification|100%||
|Blogging.Domain.Posts.Specifications.PostTitleSpecification|100%||
|Blogging.Domain.Specifications.ExpressionComposer|100%|50%|
|Blogging.Domain.Specifications.OrderClause`1|100%||
|Blogging.Domain.Specifications.Specification`1|0%||
|Blogging.Domain.Specifications.SpecificationExtensions|100%||
|Blogging.Domain.Validation.PostValidationRules|100%|100%|

</details>
<details><summary>Blogging.Repository - 96.4%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Blogging.Repository**|**96.4%**|**87.5%**|
|Blogging.Repository.Configuration.BlogPostEntityConfigurator|100%||
|Blogging.Repository.Configuration.CommentEntityConfigurator|100%||
|Blogging.Repository.DependencyInjection.AddBloggingRepositoryExtension|100%||
|Blogging.Repository.DependencyInjection.UseBloggingDatabaseAsyncExtension|100%|100%|
|Blogging.Repository.Options.BlogDatabaseOptions|100%||
|Blogging.Repository.Persistence.BlogDbContext|100%||
|Blogging.Repository.Persistence.BlogDbContextFactory|100%||
|Blogging.Repository.Persistence.Migrations.BlogDbContextModelSnapshot|100%||
|Blogging.Repository.Persistence.Migrations.InitialCreate|94%||
|Blogging.Repository.Posts.BlogPostRepository|94.1%|85.7%|

</details>
