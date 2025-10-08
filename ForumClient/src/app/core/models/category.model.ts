export interface Category {
  id: number;
  name: string;
  description: string;
  slug: string;
  parentCategoryId: number | null;
  postsCount: number;
  displayOrder: number;
  subCategories: Category[];
  createdOn: string;
  modifiedOn?: string | null;
  isDeleted: boolean;
  deletedOn?: string | null;
}
