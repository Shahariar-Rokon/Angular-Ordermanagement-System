import { Item } from "./item";
import { Recipe } from "./recipe";

export interface RecipeItems {
recipeItemId:number,
quantity:number,
recipeId:number,
recipe?:Recipe,
itemId:number,
item?:Item
}

   