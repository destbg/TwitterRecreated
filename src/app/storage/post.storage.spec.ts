import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { PostStorage } from './post.storage';

describe('PostStorage', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const storage: PostStorage = TestBed.get(PostStorage);
    expect(storage).toBeTruthy();
  });
});
