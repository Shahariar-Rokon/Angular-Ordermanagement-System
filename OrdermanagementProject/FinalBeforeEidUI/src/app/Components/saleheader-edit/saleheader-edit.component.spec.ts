import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleheaderEditComponent } from './saleheader-edit.component';

describe('SaleheaderEditComponent', () => {
  let component: SaleheaderEditComponent;
  let fixture: ComponentFixture<SaleheaderEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SaleheaderEditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SaleheaderEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
