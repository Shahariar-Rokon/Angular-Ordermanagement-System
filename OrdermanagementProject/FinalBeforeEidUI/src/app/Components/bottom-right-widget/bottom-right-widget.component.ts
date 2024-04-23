import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { SaleHeader } from '../../Models/saleheader';
import { DailyMenu } from '../../Models/daily-menu';
import { SaleDetails } from '../../Models/sale-details';
import { SaleheaderService } from '../../Services/saleheader.service';
import { MenuService } from '../../Services/menu.service';
@Component({
  selector: 'app-bottom-right-widget',
  templateUrl: './bottom-right-widget.component.html',
  styleUrl: './bottom-right-widget.component.css'
})

export class BottomRightWidgetComponent implements OnInit {
  saleHeaderList: SaleHeader[] = [];
  saleDetailList: SaleDetails[] = [];
  dailyMenuList: DailyMenu[] = [];
  chart: any;
  constructor(private service: SaleheaderService, private dailymenuservice: MenuService){}
  ngOnInit(): void {
    this.service.getSales().subscribe(x => {
      this.saleHeaderList = x;
      this.initializeChart();
      console.log(x);
    })
    this.dailymenuservice.getDailyMenu().subscribe(x => {
      this.dailyMenuList = x;
      this.initializeChart();
    });
  }
  
  
  getTopThreeSoldItems(): any[] {
    const itemQuantities: { [key: string]: number } = {};
    for (const saleHeader of this.saleHeaderList) {
      for (const saleDetail of saleHeader.saleDetails) {
        const menuItem = this.dailyMenuList.find(menu => menu.dailyMenuId === saleDetail.dailyMenuId);
        if (menuItem && menuItem.recipe?.recipeName) {
          const itemName = menuItem.recipe?.recipeName;
          itemQuantities[itemName] = (itemQuantities[itemName] || 0) + saleDetail.quantity;
        }
      }
    }
    const sortedItems = Object.entries(itemQuantities).sort((a, b) => b[1] - a[1]).slice(0, 3);
    return sortedItems.map(([name, quantity]) => ({ name, quantity }));
  }
  getRandomColor(): string {
    return `#${Math.floor(Math.random() * 16777215).toString(16)}`;
  }

  initializeChart() {
    if (this.saleHeaderList.length && this.dailyMenuList.length) {
      const topThreeSoldItems = this.getTopThreeSoldItems();
      const categories = topThreeSoldItems.map(item => item.name);
      const data = topThreeSoldItems.map(item => ({ name: item.name, y: item.quantity, color: this.getRandomColor() }));
      this.chart = new Chart({
        chart: { type: 'bar', height: 225 },
        title: { text: 'Top 3 Sold Items' },
        xAxis: { categories },
        yAxis: { title: { text: 'Quantity' } },
        series: [{ type: 'bar', showInLegend: false, data }],
        credits: { enabled: false }
      });
    }
  }

}
