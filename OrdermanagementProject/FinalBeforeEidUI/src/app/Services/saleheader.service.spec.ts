import { TestBed } from '@angular/core/testing';

import { SaleheaderService } from './saleheader.service';

describe('SaleheaderService', () => {
  let service: SaleheaderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SaleheaderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
