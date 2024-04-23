import { Component } from '@angular/core';
import { MenuService } from '../../Services/menu.service';
import { SaleheaderService } from '../../Services/saleheader.service';
import { DailyMenu } from '../../Models/daily-menu';
import { SaleHeader } from '../../Models/saleheader';

@Component({
  selector: 'app-bottomleft-widget',
  templateUrl: './bottomleft-widget.component.html',
  styleUrl: './bottomleft-widget.component.css'
})
export class BottomleftWidgetComponent {
  dailyMenuList: DailyMenu[] = [];
  saleHeaderList: SaleHeader[] = [];
  lastThreeRecords:SaleHeader[]=[];
  constructor(private service: SaleheaderService, private dailymenuservice: MenuService){}
  ngOnInit(): void {
    this.service.getSales().subscribe(x => {
      this.saleHeaderList = x;
      this.lastThreeRecords = this.saleHeaderList.slice(-3).reverse();
      console.log(x);
    })
    this.dailymenuservice.getDailyMenu().subscribe(x => {
      this.dailyMenuList = x;
    });
  }
  isFutureDate(date: string): boolean {
    let today = new Date();
    today.setHours(0, 0, 0, 0);  
    let itemDate = new Date(date);
    return itemDate > today;
  }  
}
