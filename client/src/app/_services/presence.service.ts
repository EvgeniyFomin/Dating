import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})

export class PresenceService {
  private hubConnection?: HubConnection;
  private toastr = inject(ToastrService);
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
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => console.log(error))
    }
  }
}
