import { TestBed } from '@angular/core/testing';

import { MessageService } from './message.service';
import { HttpClientModule } from '@angular/common/http';

describe('MessageService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: MessageService = TestBed.get(MessageService);
    expect(service).toBeTruthy();
  });
});
