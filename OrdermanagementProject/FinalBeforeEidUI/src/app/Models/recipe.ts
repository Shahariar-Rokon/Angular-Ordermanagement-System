import { DailyMenu } from "./daily-menu";
import { RecipeItems } from "./recipe-items";

export interface Recipe {
  recipeId: number;
  recipeName?: string;
  recipeItems?: RecipeItems[];
  dailyMenus?: DailyMenu[];
}
export interface RecipeDTO{
  recipeName:string,
  recipeItems:string,
}