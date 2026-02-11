export interface ITask {
    id: number;
    text: string;
    description?: string;
    completed?: boolean;
    categoryId?: number | null;
    category?: ICategory | null;
}

export interface ICategory {
  id: number;
  name: string;
  color?: string | null;
}