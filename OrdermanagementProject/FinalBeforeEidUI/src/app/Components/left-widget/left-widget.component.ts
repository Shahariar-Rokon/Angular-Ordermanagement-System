import { Component } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { SaleHeader } from '../../Models/saleheader';
import { SaleDetails } from '../../Models/sale-details';
import { DailyMenu } from '../../Models/daily-menu';
import { SaleheaderService } from '../../Services/saleheader.service';
import { MenuService } from '../../Services/menu.service';
@Component({
  selector: 'app-left-widget',
  templateUrl: './left-widget.component.html',
  styleUrl: './left-widget.component.css'
})
export class LeftWidgetComponent {
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
  
  initializeChart() {
 
    const groupedByMonth = this.saleHeaderList.reduce((acc: Record<string, any>, saleHeader) => {
      const month = new Date(saleHeader.saleDate).getMonth();
      if (!acc[month]) {
        acc[month] = [];
      }
      acc[month].push(...saleHeader.saleDetails);
      return acc;
    }, {});
 
    const monthlyQuantities = Object.entries(groupedByMonth).map(([month, saleDetails]) => {
      const totalQuantity = saleDetails.reduce((sum: number, detail: { quantity: number }) => sum + detail.quantity, 0);
      return { month: parseInt(month, 10), totalQuantity };
    });
  

    const uniqueMonths = [...new Set(monthlyQuantities.map(({ month }) => month))].sort((a, b) => a - b);
  
  
    const monthCategories = uniqueMonths.map(month => {
      const monthName = new Date(0, month).toLocaleString('default', { month: 'long' });
      return monthName;
    });
  

    const quantitySeries = monthlyQuantities.map(({ month, totalQuantity }) => totalQuantity);
  
    this.chart = new Chart({
      chart: {
        type: 'line',
        height: 325
      },
      title: {
        text: 'Monthly Total Quantity Sold'
      },
      xAxis: {
        categories: monthCategories
      },
      yAxis: {
        title: {
          text: 'Quantity'
        }
      },
      series: [
        {
          name: 'Total Quantity',
          type: 'line',
          data: quantitySeries
        }
      ],
      credits: {
        enabled: false
      }
    });
  }
  }
