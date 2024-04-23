import { Component } from '@angular/core';
import {
  faDashboard,
  faLocation,
  faShop,
  faBox,
  faMoneyBill,
  faChartBar,
  faContactBook,
  faHand,
  faFaceLaugh,
  faNewspaper
} from '@fortawesome/free-solid-svg-icons';
import { SaleheaderService } from '../../Services/saleheader.service';
import { SaleHeader } from '../../Models/saleheader';
import { MenuService } from '../../Services/menu.service';
import { DailyMenu } from '../../Models/daily-menu';

@Component({
  selector: 'app-topsection',
  templateUrl: './topsection.component.html',
  styleUrl: './topsection.component.css'
})
export class TopsectionComponent {
  faDashboard = faDashboard;
  faLocation = faLocation;
  faShop = faShop;
  faBox = faBox;
  faMoneyBill = faMoneyBill;
  faChartBar = faChartBar;
  faContactBook = faContactBook;
  faHand = faHand;
  faFaceLaugh=faFaceLaugh;
  faNewspaper=faNewspaper;
  saleHeaderList:SaleHeader[]=[];
  dailyMenuList:DailyMenu[]=[];
  totalBill:number=0;
  totalMenu:number=0;
  totalCustomer:number=0;
  totalQuantity:number=0;
constructor(private salesService:SaleheaderService,private dailymenuservice:MenuService){}
  ngOnInit(): void {
    this.salesService.getSales().subscribe(x => {
      this.saleHeaderList = x;
      this.calculateTotalBill();
      this.countCustomer();
      this.countQuantity();
      console.log(x);
    });
    this.dailymenuservice.getDailyMenu().subscribe(x => {
      this.dailyMenuList = x;
      this.countMenu();
    });
  }
  
  calculateTotalBill() {
    this.totalBill = this.saleHeaderList.reduce((sum, saleHeader) => sum + saleHeader.totalBill, 0);
  }
  countMenu(){
    this.totalMenu = this.dailyMenuList.length; 
}
countCustomer(){
  this.totalCustomer = this.saleHeaderList.length;
}
countQuantity(){
  this.totalQuantity = this.saleHeaderList.reduce((total, x) =>
    total + x.saleDetails.reduce((subTotal, y) => subTotal + y.quantity, 0)
  , 0);
}

}
