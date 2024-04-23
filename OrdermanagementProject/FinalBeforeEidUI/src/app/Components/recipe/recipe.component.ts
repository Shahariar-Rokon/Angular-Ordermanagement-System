import { Component, OnInit, ViewChild } from '@angular/core';
import { RecipeService } from '../../Services/recipe.service';
import { Recipe, RecipeDTO } from '../../Models/recipe';
import { RecipeItems } from '../../Models/recipe-items';
import { Item } from '../../Models/item';
import { ItemService } from '../../Services/item.service';
import { MatTabGroup } from '@angular/material/tabs';
import { RecipeItemService } from '../../Services/recipe-item.service';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrl: './recipe.component.css'
})
export class RecipeComponent implements OnInit {

  @ViewChild(MatTabGroup) tabGroup!: MatTabGroup;

  recipeList: Recipe[] = [];
  recipeitemList: RecipeItems[] = [];
  recipeItemListAdd: RecipeItems[] = []
  recipeItemObj: RecipeItems = { recipeItemId: 0, recipeId: 0, itemId: 0, recipe: { recipeId: 0, recipeName: "", recipeItems: [] }, item: { itemId: 0, name: "", unit: "", type: "" }, quantity: 0 }
  recipeObj: RecipeDTO = { recipeName: "", recipeItems: "" }
  itemList: Item[] = [];

  editMode = false;
  editedRecipe: Recipe = { recipeId: 0, recipeName: '', recipeItems: [] };

  constructor(private service: RecipeService, private itemService: ItemService, private recipeItemService: RecipeItemService) { }

  ngOnInit(): void {
    this.service.getRecipes().subscribe(x => {
      this.recipeList = x,
        this.itemService.getItems().subscribe(z => {
          this.itemList = z;
        })
      this.recipeItemService.getRecipeItems().subscribe(r => {
        this.recipeitemList = r;
        console.log(this.recipeitemList);
      })
    });
  }

  addRecipeItems() {
    this.recipeItemListAdd.unshift(this.recipeItemObj);
    this.recipeItemObj = { recipeItemId: 0, recipeId: 0, itemId: 0, recipe: { recipeId: 0, recipeName: "", recipeItems: [] }, item: { itemId: 0, name: "", unit: "", type: "" }, quantity: 0 };
    
  }

  deleteDetail(exp: RecipeItems, arry: any[]) {
    const objWithTitle = arry.findIndex((obj) => obj === exp);
    if (objWithTitle > -1) {
      arry.splice(objWithTitle, 1);
    }
  }

  postRecipe() {
    const recipeItemsJson = JSON.stringify(this.recipeItemListAdd);
    const recipeDTO: RecipeDTO = {
      recipeName: this.recipeObj.recipeName,
      recipeItems: recipeItemsJson
    };
    this.service.postRecipe(recipeDTO).subscribe({
      next: (response) => {
        console.log(response);
        this.recipeObj = { recipeName: "", recipeItems: "" };
        this.service.getRecipes().subscribe(x => {
          this.recipeList = x,
            this.itemService.getItems().subscribe(z => {
              this.itemList = z;
            })
          this.recipeItemService.getRecipeItems().subscribe(r => {
            this.recipeitemList = r;
            console.log(this.recipeitemList);
          })
        });
        this.tabGroup.selectedIndex = 1;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  editRecipe(recipe: Recipe): void {
    this.editMode = true;
    this.editedRecipe = { ...recipe };
  }

 

  cancelEdit(): void {
    this.editMode = false;
    this.editedRecipe = { recipeId: 0, recipeName: '', recipeItems: [] };
  }

  deleteRecipe(recipe: Recipe): void {
    this.service.deleteRecipe(recipe.recipeId).subscribe({
      next: (response) => {
        console.log(response);
        this.service.getRecipes().subscribe(x => {
          this.recipeList = x,
            this.itemService.getItems().subscribe(z => {
              this.itemList = z;
            })
          this.recipeItemService.getRecipeItems().subscribe(r => {
            this.recipeitemList = r;
            console.log(this.recipeitemList);
          })
        });
      },
      error: (error) => {
        console.error(error);
      }
    });
  }


}



