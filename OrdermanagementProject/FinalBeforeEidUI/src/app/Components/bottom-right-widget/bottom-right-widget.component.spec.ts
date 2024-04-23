import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BottomRightWidgetComponent } from './bottom-right-widget.component';

describe('BottomRightWidgetComponent', () => {
  let component: BottomRightWidgetComponent;
  let fixture: ComponentFixture<BottomRightWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BottomRightWidgetComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BottomRightWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
