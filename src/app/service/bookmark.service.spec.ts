import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { BookmarkService } from './bookmark.service';

describe('BookmarkService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: BookmarkService = TestBed.get(BookmarkService);
    expect(service).toBeTruthy();
  });
});
