import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { MessageStorage } from './message.storage';
import { HttpClientModule } from '@angular/common/http';

describe('MessageStorage', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([]), HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: MessageStorage = TestBed.get(MessageStorage);
    expect(service).toBeTruthy();
  });
});
