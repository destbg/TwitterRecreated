import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RepostService } from './repost.service';

describe('RepostService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: RepostService = TestBed.get(RepostService);
    expect(service).toBeTruthy();
  });
});
