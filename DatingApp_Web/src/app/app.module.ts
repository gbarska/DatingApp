import { BrowserModule, HAMMER_GESTURE_CONFIG, HammerGestureConfig } from '@angular/platform-browser';
import { BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BsDropdownModule, TabsModule, PaginationModule, ButtonsModule, ModalModule, BsDatepickerModule } from 'ngx-bootstrap';
import { AppComponent } from './app.component';
// import { AuthInterceptorService } from './auth-interceptor.service';
// import { LoggingInterceptorService } from './logging-interceptor.service';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListComponent } from './list/list.component';
import { MessagesComponent } from './messages/messages.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtModule } from '@auth0/angular-jwt';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { AlertifyService } from './_services/alertify.service';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { UserService } from './_services/user.service';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { NgxGalleryModule } from 'ngx-gallery';
import { TimeAgoPipe } from 'time-ago-pipe'
import { ModalComponent } from './modal/modal.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { PhotoManagementComponent } from './admin/photo-management/photo-management.component';
import { AdminModalComponent } from './admin/admin-modal/admin-modal.component';
import { ChatComponent } from './chat/chat.component';
import { ChatService } from './_services/chat.service';
import { AdminService } from './_services/admin.service';
import { HasRoleDirective } from './_directives/hasRole.directive';

export function tokenGetter(){
   return localStorage.getItem('token');
}

export class CustomHammerConfig extends HammerGestureConfig{
   overrides = {
      pinch: { enable: false},
      rotate: { enable: false}
   }
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      TimeAgoPipe,
      ModalComponent,
      PhotoEditorComponent,
      MemberMessagesComponent,
      ChatComponent,
      AdminPanelComponent,
      HasRoleDirective,
      UserManagementComponent,
      PhotoManagementComponent,
      AdminModalComponent
   ],
   entryComponents:[
      AdminModalComponent
   ],
   imports: [
      BrowserModule,
      FormsModule,
      BrowserAnimationsModule,
      ReactiveFormsModule,
      HttpClientModule,
      FileUploadModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      PaginationModule.forRoot(),
      ButtonsModule.forRoot(),
      ModalModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      BsDatepickerModule.forRoot(),
      JwtModule.forRoot({
         config: {
            tokenGetter: tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      }),
      TabsModule.forRoot(),
      NgxGalleryModule
   ],
      providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuard,
      UserService,
      MemberDetailResolver,
      MemberListResolver,
      ListsResolver,
      MessagesResolver,
      MemberEditResolver,
      {provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig },
      PreventUnsavedChanges,
      ChatService,
      AdminService
      ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule {}
