import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BottomleftWidgetComponent } from './bottomleft-widget.component';

describe('BottomleftWidgetComponent', () => {
  let component: BottomleftWidgetComponent;
  let fixture: ComponentFixture<BottomleftWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BottomleftWidgetComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BottomleftWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
