import { Component, OnInit, ViewChild } from '@angular/core';
import { DailyMenu, DailyMenuDTO, DailyMenuDTOAfter } from '../../Models/daily-menu';
import { MenuService } from '../../Services/menu.service';
import { Recipe } from '../../Models/recipe';
import { RecipeService } from '../../Services/recipe.service';
import { MatTabGroup } from '@angular/material/tabs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-daily-menu',
  templateUrl: './daily-menu.component.html',
  styleUrl: './daily-menu.component.css'
})
export class DailyMenuComponent implements OnInit {

  @ViewChild(MatTabGroup) tabGroup!: MatTabGroup;

  dailyMenuList: DailyMenu[] = [];
  dailyMenuObj: DailyMenuDTO = { demandQuantity: 0, recipeId: 0, price: 0 }
  patchDailyMenuobj: DailyMenuDTOAfter = { cookedQuantity: 0 }
  dmenuListFromDto: DailyMenuDTO[] = [];
  recipes: Recipe[] = [];
  editing: boolean[] = [];

  constructor(private service: MenuService,private recipeService:RecipeService) { }

  ngOnInit(): void {
    this.service.getDailyMenu().subscribe(x => {
      this.dailyMenuList = x;
    })
    this.recipeService.getRecipes().subscribe(x=>{
      this.recipes =x;
    })
  }

  addDailyMenu() {
    const strExpObj = JSON.stringify(this.dailyMenuObj);
    const obj = JSON.parse(strExpObj);
    this.service.postDailyMenu(obj).subscribe(x => {
      console.log(x);
      this.dailyMenuObj = { demandQuantity: 0, recipeId: 0, price: 0 };
      this.service.getDailyMenu().subscribe(x => {
        this.dailyMenuList = x;
      })
      this.recipeService.getRecipes().subscribe(x=>{
        this.recipes =x;
      })
      this.tabGroup.selectedIndex = 1;
    })
  }

  submitPatch(id: number) {
    this.service.patchDailyMenu(id, this.patchDailyMenuobj).subscribe(x => {
      this.patchDailyMenuobj = { cookedQuantity: 0 };
      this.service.getDailyMenu().subscribe(updatedData => {
        this.dailyMenuList = updatedData;
      });
    })
  }
  updateDailyMenu(index: number, dailyMenu: DailyMenu) {
    this.service.putDailyMenu(dailyMenu.dailyMenuId, dailyMenu).subscribe({
      next: response => {
        console.log(response);
        this.editing[index] = false;
        this.service.getDailyMenu().subscribe(updatedData => {
          this.dailyMenuList = updatedData;
        });
      },
      error: error => {
        console.log(error);
      }
    });
  }
  cancelEdit(index: number):void{
    this.editing[index] = false;
  }
  deleteMenu(id:number){
    this.service.DeleteDailyMenu(id).subscribe(x=>{
      console.log("Deleted");
      this.service.getDailyMenu().subscribe(updatedData => {
        this.dailyMenuList = updatedData;
      });
    })
  }

}
