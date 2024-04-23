import { Recipe } from "./recipe";
import { SaleDetails } from "./sale-details";

export interface DailyMenu {
  dailyMenuId: number;
  dailyMenuDate: Date;
  demandQuantity: number;
  cookedQuantity: number;
  servingQuantity: number;
  price: number;
  recipeId: number;
  recipe?: Recipe;
  saleDetails: SaleDetails[];
  
}

export interface DailyMenuDTO {
  demandQuantity: number;
  recipeId: number;
  price: number;
}

export interface DailyMenuDTOAfter {
  cookedQuantity: number;

}