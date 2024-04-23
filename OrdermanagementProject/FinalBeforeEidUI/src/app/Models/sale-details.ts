import { DailyMenu } from "./daily-menu";
import { SaleHeader } from "./saleheader";

export interface SaleDetails {
  id: number;
  saleHeaderId: number;
  dailyMenuId: number;
  quantity: number;
  saleHeader: SaleHeader | null;
  dailyMenu: DailyMenu | null;
}
