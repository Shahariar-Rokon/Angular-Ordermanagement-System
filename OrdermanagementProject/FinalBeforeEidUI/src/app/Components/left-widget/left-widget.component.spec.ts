import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftWidgetComponent } from './left-widget.component';

describe('LeftWidgetComponent', () => {
  let component: LeftWidgetComponent;
  let fixture: ComponentFixture<LeftWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LeftWidgetComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LeftWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
