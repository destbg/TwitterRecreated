import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { TagService } from './tag.service';

describe('TagService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: TagService = TestBed.get(TagService);
    expect(service).toBeTruthy();
  });
});
