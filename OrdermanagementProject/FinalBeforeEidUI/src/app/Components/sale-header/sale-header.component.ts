import { Component, OnInit, ViewChild } from '@angular/core';
import { SaleHeader } from '../../Models/saleheader';
import { SaleheaderService } from '../../Services/saleheader.service';
import { SaleDetails } from '../../Models/sale-details';
import { RecipeService } from '../../Services/recipe.service';
import { MenuService } from '../../Services/menu.service';
import { DailyMenu } from '../../Models/daily-menu';
import { MatTabGroup } from '@angular/material/tabs';
import { Recipe } from '../../Models/recipe';
import { setPdfFonts } from '../../PDFMaker/pdfUtils';


@Component({
  selector: 'app-sale-header',
  templateUrl: './sale-header.component.html',
  styleUrl: './sale-header.component.css'
})

export class SaleHeaderComponent implements OnInit {

  @ViewChild(MatTabGroup) tabGroup!: MatTabGroup;

  saleHeaderList: SaleHeader[] = [];
  saleHeaderobj: SaleHeader = { id: 0, invoiceNumber: "", customerName: "", customerEmail: "", customerPhone: "", totalPrice: 0, vat: 0, totalBill: 0, saleDate: new Date(), saleDetails: [] }
  saleDetailList: SaleDetails[] = [];
  saleDetailsObj: SaleDetails = { id: 0, dailyMenuId: 0, saleHeaderId: 0, quantity: 0, saleHeader: null, dailyMenu: null }
  dailyMenuList: DailyMenu[] = [];
  editing: boolean[] = [];
  constructor(private service: SaleheaderService, private dailymenuservice: MenuService) { }
  ngOnInit(): void {
    this.service.getSales().subscribe(x => {
      this.saleHeaderList = x;
      console.log(x);
    })
    this.dailymenuservice.getDailyMenu().subscribe(x => {
      this.dailyMenuList = x;
    });
  }
  addMenus() {
    if (this.saleDetailsObj.quantity != 0) {
      const strExpObj = JSON.stringify(this.saleDetailsObj);
      const obj = JSON.parse(strExpObj);
      this.saleDetailList.unshift(obj);
      console.log(this.saleDetailList);
      this.saleDetailsObj = { id: 0, dailyMenuId: 0, saleHeaderId: 0, quantity: 0, saleHeader: null, dailyMenu: null }
    }
  }
  
  deleteDetail(exp: SaleDetails, arry: any[]) {
    const objWithTitle = arry.findIndex((obj) => obj === exp);
    if (objWithTitle > -1) {
      arry.splice(objWithTitle, 1);
    }
  }

  
  addSales(): void {
    this.saleHeaderobj.saleDetails = this.saleDetailList;
    this.service.postSales(this.saleHeaderobj).subscribe(x => {
      console.log(this.saleHeaderobj);
      this.saleHeaderobj = { id: 0, invoiceNumber: "", customerName: "", customerEmail: "", customerPhone: "", totalPrice: 0, vat: 0, totalBill: 0, saleDate: new Date(), saleDetails: [] };
      this.service.getSales().subscribe(x => {
        this.saleHeaderList = x;
        console.log(x);
        this.generateInvoice(this.saleHeaderList[this.saleHeaderList.length - 1]);
      })
      this.dailymenuservice.getDailyMenu().subscribe(x => {
        this.dailyMenuList = x;
      });
      this.tabGroup.selectedIndex = 1;
    });
  }
  deleteSales(id: number) {
    if (window.confirm('Are you sure?')) {
      this.service.deleteSales(id).subscribe(x => {
        console.log("Deleted");
        this.service.getSales().subscribe(updatedData => {
          this.saleHeaderList = updatedData;
        });
      })
    }
  }
  generateInvoice(saleHeader:SaleHeader): void {
    const invoiceContent = [
      { text: 'INVOICE', fontSize: 18, bold: true, alignment: 'center', margin: [0, 0, 0, 20] },
      { text: `Customer: ${saleHeader.customerName}`, fontSize: 14, bold: true },
      { text: `Email: ${saleHeader.customerEmail}`, fontSize: 12 },
      { text: `Phone: ${saleHeader.customerPhone}`, fontSize: 12 },
      { text: `Invoice Number: ${saleHeader.invoiceNumber}`, fontSize: 12 },
      { text: `Date: ${this.saleHeaderobj.saleDate.toString()}`, fontSize: 12 },
      { canvas: [{ type: 'line', x1: 0, y1: 5, x2: 595 - 2 * 40, y2: 5, lineWidth: 0.5 }] },
      {
        table: {
          widths: ['*', '*', '*', '*'],
          body: [
            ['Item', 'Quantity', 'Price', 'Total'],
            ...saleHeader.saleDetails.map(detail => {
              const dailyMenu = this.dailyMenuList.find(menu => menu.dailyMenuId === detail.dailyMenuId);
              const itemPrice = dailyMenu ? dailyMenu.price : 0;
              const totalPrice = itemPrice * detail.quantity;
              return [`${dailyMenu ? dailyMenu.recipe?.recipeName : ''}`, `${detail.quantity}`, `$${itemPrice.toFixed(2)}`, `$${totalPrice.toFixed(2)}`];
            })
          ]
        },
        layout: 'lightHorizontalLines'
      },

      { text: `Total: $${saleHeader.totalPrice.toFixed(2)}`, fontSize: 14, bold: true, margin: [0, 10, 0, 10] },
      { text: `VAT: $${saleHeader.vat.toFixed(2)}`, fontSize: 14, bold: true },
      { text: `Grand Total: $${saleHeader.totalBill.toFixed(2)}`, fontSize: 16, bold: true },
    ];
    const pdfMakeInstance = setPdfFonts();
    const documentDefinition = {
      content: invoiceContent
    };
    pdfMakeInstance.createPdf(documentDefinition as any).open();
  }
  isPastDate(date: string): boolean {
    let today = new Date();
    today.setHours(0, 0, 0, 0);  
    let itemDate = new Date(date);
    return itemDate < today;
  }
  isFutureDate(date: string): boolean {
    let today = new Date();
    today.setHours(0, 0, 0, 0);  
    let itemDate = new Date(date);
    return itemDate > today;
  }
  
}

