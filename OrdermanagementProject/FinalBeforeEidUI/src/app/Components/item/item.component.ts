import { Component, OnInit, ViewChild, viewChild } from '@angular/core';
import { Item } from '../../Models/item';
import { ItemService } from '../../Services/item.service';
import { Router } from '@angular/router';
import { response } from 'express';
import { MatTabGroup } from '@angular/material/tabs';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrl: './item.component.css'
})
export class ItemComponent implements OnInit {
  
  @ViewChild(MatTabGroup) tabGroup!: MatTabGroup;

  items: Item[] = [];
  newItem: Item = {itemId: 0, name: '', unit: '', type: ''};
  editingItem: boolean[] = [];

  constructor (private itemService: ItemService, private router: Router){}

  ngOnInit(): void {
    this.getItems();
  }

  getItems() {
    this.itemService.getItems().subscribe(items => this.items = items);
  }


  addItem(): void {
    this.itemService.postItem(this.newItem).subscribe(item => {
      console.log(item);
      this.newItem = {itemId: 0, name: '', unit: '', type: ''};
      this.getItems();
      this.tabGroup.selectedIndex = 1;
    });
  }

  updateItem(index: number, item: Item) {
    this.itemService.updateItem(item.itemId, item).subscribe({
      next: response => {
        console.log(response);
        this.editingItem[index] = false;
        this.itemService.getItems().subscribe(x => {
          this.items = x;
        });
      },
      error: error => {
        console.log(error);
      }
    });
  }
  
  
  cancelEdit(index: number):void{
    this.editingItem[index] = false;
  }


  deleteItem(itemId: number): void{
    this.itemService.deleteItem(itemId).subscribe(() => {
      this.items = this.items.filter(item => item.itemId !== itemId);
      this.itemService.getItems().subscribe(x=>{
        this.items = x;
      })
    });
  }

}
