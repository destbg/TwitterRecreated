import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RightSideStorage } from './right-side.storage';

describe('RightSideStorage', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: RightSideStorage = TestBed.get(RightSideStorage);
    expect(service).toBeTruthy();
  });
});
