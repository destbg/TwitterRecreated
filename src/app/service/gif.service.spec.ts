import { TestBed } from '@angular/core/testing';

import { GifService } from './gif.service';
import { HttpClientModule } from '@angular/common/http';

describe('GifService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: GifService = TestBed.get(GifService);
    expect(service).toBeTruthy();
  });
});
