import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SaleheaderService } from '../../Services/saleheader.service';
import { SaleHeader } from '../../Models/saleheader';
import { SaleDetails } from '../../Models/sale-details';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuService } from '../../Services/menu.service';
import { DailyMenu } from '../../Models/daily-menu';
import { DatePipe } from '@angular/common';
import { SaleDetailsService } from '../../Services/sale-details.service';
import { RecipeService } from '../../Services/recipe.service';

@Component({
  selector: 'app-saleheader-edit',
  templateUrl: './saleheader-edit.component.html',
  styleUrl: './saleheader-edit.component.css'
})
export class SaleheaderEditComponent implements OnInit {
  saleHeaderList: SaleHeader[] = [];
  saleHeaderobj: SaleHeader = { id: 0, invoiceNumber: "", customerName: "", customerEmail: "", customerPhone: "", totalPrice: 0, vat: 0, totalBill: 0, saleDate: new Date(), saleDetails: [] }
  saleDetailList: SaleDetails[] = [];
  saleDetailsObj: SaleDetails = { id: 0, dailyMenuId: 0, saleHeaderId: 0, quantity: 0, saleHeader: null, dailyMenu: null }
  dailyMenuList:DailyMenu[]=[];
  editing: boolean[] = [];
constructor(private cdr: ChangeDetectorRef,private recipeService:RecipeService,private service: SaleheaderService,private route:ActivatedRoute,private router:Router,private dailymenuservice:MenuService,private detailservice:SaleDetailsService){}
 
ngOnInit(): void {
  this.route.paramMap.subscribe({
    next:(params)=>{
      const id = params.get('id');
      if(id){
        this.service.getSalesById(Number(id)).subscribe({
          next:(res)=>{
            if(res.saleDetails){
              this.saleDetailList = res.saleDetails;
              this.saleHeaderobj ={
                id:res.id,
                invoiceNumber:res.invoiceNumber,
                saleDate:new Date(res.saleDate),
                totalBill:res.totalBill,
                totalPrice:res.totalPrice,
                vat:res.vat,
                customerName:res.customerName,
                customerEmail:res.customerEmail,
                customerPhone:res.customerPhone,
                saleDetails:this.saleDetailList
            }
            }
          }
        })
      }
    }
  })
  this.dailymenuservice.getDailyMenu().subscribe(x=>{
    this.dailyMenuList=x;
        });
}
deleteDetail(exp: SaleDetails, arry: any[]) {
  const objWithTitle = arry.findIndex((obj) => obj === exp);
  if (objWithTitle > -1) {  
    arry.splice(objWithTitle, 1);  
  }
}

addMenus() {
  if (this.saleDetailsObj.quantity != 0) {
    const strExpObj = JSON.stringify(this.saleDetailsObj);
    const obj = JSON.parse(strExpObj);
    this.saleDetailList.unshift(obj);
    this.saleDetailsObj = { id: 0, dailyMenuId: 0, saleHeaderId: 0, quantity: 0, saleHeader: null, dailyMenu: null }
  }
}
updateSaleHeader(){
  this.service.putSales(this.saleHeaderobj.id, this.saleHeaderobj)
    .subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigate(['SaleHeader']);
      },
      error: (error) => {
        console.log(error);
      },
    });
}





}

