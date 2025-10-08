export interface PostSummary {
  id: number;
  title: string;
  slug: string;
  authorName: string;
  categoryName: string;
  viewCount: number;
  votesCount: number;
  commentsCount: number;
  createdOn: string;
  modifiedOn?: string | null;
  isDeleted: boolean;
  deletedOn?: string | null;
}

export interface Post extends PostSummary {
  content: string;
  authorId: string;
  categoryId: number;
  tags: string[];
}

export interface GetPostsParams {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  descending?: boolean;
}

export interface CreatePostPayload {
  title: string;
  content: string;
  categoryId: number;
  tags: string[];
}

export type UpdatePostPayload = CreatePostPayload;
