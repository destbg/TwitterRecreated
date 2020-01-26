import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { VoteService } from './vote.service';

describe('VoteService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: VoteService = TestBed.get(VoteService);
    expect(service).toBeTruthy();
  });
});
