import { Component, OnInit } from '@angular/core';
import { RecipeService } from '../../Services/recipe.service';
import { RecipeItemService } from '../../Services/recipe-item.service';
import { ActivatedRoute, Router,} from '@angular/router';
import { RecipeItems } from '../../Models/recipe-items';
import { Recipe, RecipeDTO } from '../../Models/recipe';
import { Item } from '../../Models/item';
import { ItemService } from '../../Services/item.service';

@Component({
  selector: 'app-recipe-edit',
  templateUrl: './recipe-edit.component.html',
  styleUrl: './recipe-edit.component.css'
})

export class RecipeEditComponent implements OnInit {
  constructor(private service: RecipeService, private itemService: ItemService, private recipeItemService: RecipeItemService,private route:ActivatedRoute,private router:Router) { }
  recipeList: Recipe[] = [];
  recipeitemList: RecipeItems[] = [];
  recipeItemListAdd: RecipeItems[] = []
  recipeItemObj: RecipeItems = { recipeItemId: 0, recipeId: 0, itemId: 0, recipe: { recipeId: 0, recipeName: "", recipeItems: [] }, item: { itemId: 0, name: "", unit: "", type: "" }, quantity: 0 }
  recipeObj: RecipeDTO = { recipeName: "", recipeItems: "" }
  itemList: Item[] = [];
  recipeId: number=0;
  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        this.recipeId = Number(id);
        if (id) {
          this.service.getRecipeById(this.recipeId).subscribe({
            next: (res) => {
              if (res.recipeItems && res.recipeName) {
                this.recipeitemList = res.recipeItems;
                this.recipeItemListAdd = [...res.recipeItems]; 
                const recipeItemsJson = JSON.stringify(res.recipeItems);
                this.recipeObj = {
                  recipeName: res.recipeName,
                  recipeItems: recipeItemsJson
                };
              }
            }
          });
        }
      }
    });
  
    this.itemService.getItems().subscribe(z => {
      this.itemList = z;
    });
  
    this.recipeItemService.getRecipeItems().subscribe(r => {
      this.recipeitemList = r;
      console.log(this.recipeitemList);
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
  updateRecipe() {
    const recipeItemsJson = JSON.stringify(this.recipeItemListAdd);
    const recipeDTO: RecipeDTO = {
      recipeName: this.recipeObj.recipeName,
      recipeItems: recipeItemsJson
    };
    this.service.updateRecipe(this.recipeId, recipeDTO)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.router.navigate(['Recipe']);
        },
        error: (error) => {
          console.log(error);
        }
      });
  }
}

