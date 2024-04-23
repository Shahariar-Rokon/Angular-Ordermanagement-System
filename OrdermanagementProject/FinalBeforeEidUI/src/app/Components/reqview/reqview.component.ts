import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuDate, Requisition } from '../../Models/requisition';
import { RepositoryService } from '../../Services/repository.service';
import { Router } from '@angular/router';
import { DailyMenu } from '../../Models/daily-menu';
import { MenuService } from '../../Services/menu.service';
import { ItemService } from '../../Services/item.service';
import { Item } from '../../Models/item';
import { MatTabGroup } from '@angular/material/tabs';

@Component({
  selector: 'app-reqview',
  templateUrl: './reqview.component.html',
  styleUrl: './reqview.component.css',
})
export class ReqviewComponent implements OnInit {

  @ViewChild(MatTabGroup) tabGroup!: MatTabGroup;

  requisitionList: Requisition[] = [];
  menuDateList: MenuDate[] = [];
  menuDateObj: MenuDate = { dailyMenuDate: new Date() }
  dailyMenuList: DailyMenu[] = [];
  menuDate: Date = new Date();
  selectedDates: string[] = [];
  itemList: Item[] = [];
  groupedByDate: Map<string, Requisition[]> = new Map();
  constructor(private service: RepositoryService, private dailyMenuService: MenuService, private itemService: ItemService) { }

  ngOnInit(): void {
    this.dailyMenuService.getDailyMenu().subscribe(z => {
      this.dailyMenuList = z;
      console.log(z);
    })
    this.service.getAllRequisitions().subscribe((x) => {
      this.requisitionList = x.map((item) => ({
        requisitionId: item.requisitionId,
        requestedBy: item.requestedBy,
        requisitionDate: item.requisitionDate,
        requestedQuantity: item.requestedQuantity,
        itemId: item.itemId,
      }));
      this.groupedByDate = new Map();
      for (const requisition of this.requisitionList) {
        const date = requisition.requisitionDate.toString();
        const group = this.groupedByDate.get(date) || [];
        group.push(requisition);
        this.groupedByDate.set(date, group);
      }
    });
    this.itemService.getItems().subscribe(x => {
      this.itemList = x;
    });
  }
  postRequisition(): void {
    this.selectedDates.push(this.menuDateObj.dailyMenuDate.toString());
    console.log(this.menuDateObj)
    this.service.postRequisition(this.menuDateObj)
      .subscribe(x => {
        console.log(x);
        this.menuDateObj = { dailyMenuDate: new Date() }

        this.service.getAllRequisitions().subscribe((x) => {
          this.requisitionList = x.map((item) => ({
            requisitionId: item.requisitionId,
            requestedBy: item.requestedBy,
            requisitionDate: item.requisitionDate,
            requestedQuantity: item.requestedQuantity,
            itemId: item.itemId,
          }));
          this.tabGroup.selectedIndex = 1;
        });
      });
  }

  private colorMap = new Map<string, string>();

  getColor(date: string): string {
    if (!this.colorMap.has(date)) {
      // Generate a new color and store it in the map
      const color = this.generateColor();
      this.colorMap.set(date, color);
    }
    return this.colorMap.get(date)!;
  }

  generateColor(): string {
    // Generate a random color
    const color = '#' + Math.floor(Math.random() * 16777215).toString(16);
    return color;
  }






}

