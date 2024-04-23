import { SaleDetails } from "./sale-details";

export interface SaleHeader {
  id: number;
  invoiceNumber: string;
  customerName?: string;
  customerEmail?: string;
  customerPhone?: string;
  saleDate:Date;
  totalPrice: number;
  vat: number;
  totalBill: number;
  saleDetails: SaleDetails[]
}