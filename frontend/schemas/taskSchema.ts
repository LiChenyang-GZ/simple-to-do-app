import { z } from "zod";

export const taskSchema = z.object({
    text: z.string().min(1, "Task text is required"),
    description: z.string().min(5, "Description must be at least 5 characters long"),
});

export type TaskFormData = z.infer<typeof taskSchema>;