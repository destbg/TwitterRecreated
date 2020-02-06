import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { LikeService } from './like.service';

describe('LikeService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: LikeService = TestBed.get(LikeService);
    expect(service).toBeTruthy();
  });
});
