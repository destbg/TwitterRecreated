import { TestBed } from '@angular/core/testing';
import { PeerStorage } from './peer.storage';
import { HttpClientModule } from '@angular/common/http';

describe('PeerStorage', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }),
  );

  it('should be created', () => {
    const service: PeerStorage = TestBed.get(PeerStorage);
    expect(service).toBeTruthy();
  });
});
