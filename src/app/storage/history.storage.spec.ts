import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { HistoryStorage } from './history.storage';

describe('HistoryStorage', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([])],
    }),
  );

  it('should be created', () => {
    const service: HistoryStorage = TestBed.get(HistoryStorage);
    expect(service).toBeTruthy();
  });
});
