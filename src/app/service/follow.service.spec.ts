import { TestBed } from '@angular/core/testing';

import { FollowService } from './follow.service';
import { HttpClientModule } from '@angular/common/http';

describe('FollowService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: FollowService = TestBed.get(FollowService);
    expect(service).toBeTruthy();
  });
});
