import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class PresenceService {
  private hubConnection?: HubConnection;
  private toastr = inject(ToastrService);
  private router = inject(Router);
  hubsUrl = environment.hubsUrl + 'presence';
  onlineUserIds = signal<number[]>([]);

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubsUrl, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('UserIsOnline', userName => {
      this.toastr.info(userName + ' has connected')
    });

    this.hubConnection.on('UserIsOffline', userName => {
      this.toastr.warning(userName + ' has disconnected')
    });

    this.hubConnection.on('GetOnlineUsers', onlineUserIds => {
      this.onlineUserIds.set(onlineUserIds);
    })

    this.hubConnection.on('NewMessageReceived', ({ userId, knownAs }) => {
      this.toastr.info(knownAs + ' has sent you a new message. Click here to see it')
        .onTap
        .pipe(take(1))
        .subscribe(() => this.router.navigateByUrl('/members/' + userId + '?tab=Messages'))
    })
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => console.log(error))
    }
  }
}
