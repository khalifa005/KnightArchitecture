import { Component, OnInit } from '@angular/core';
import { AuthService } from '@app/@auth/services/custom-auth-service';
import { ChatBotService } from '@app/@core/utils.ts/chat-bot/chat.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit {
  title = 'chat-ui';
  text: string = "";

  messages:any;
  
  constructor(
    private authService: AuthService,
    protected chatService: ChatBotService
  ) {
    this.messages = this.chatService.loadMessages();
  }

  ngOnInit(): void {
    const token = this.authService.getAccessToken(); // Get the JWT token
  }


  uploadedFile: string | null = null;


  uploadFile(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.uploadedFile = URL.createObjectURL(file);

      // Add the uploaded file as a message
      this.messages.push({
        text: 'File uploaded!', // Informational message
        user: {
          name: 'Mahmoud khalifa',
          avatar: '',
        },
        date: new Date(),
        reply: true,
        type: 'file',
        files: [{ url: this.uploadedFile, type: 'file', icon: 'file-outline' }],
      });

      // Ensure the message list is updated and sorted correctly by date
      this.sortMessages();
    }
  }

  sortMessages() {
    // Sort messages by their date to ensure correct order
    this.messages.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
  }
  
  sendMessage(event: any) {
    let messageType = 'text'; // Default to text message
    let messageContent = event.message || ''; // Default message content
    let files = null;
    let quote = null;
    let latitude = null;
    let longitude = null;
  
    // Check if the event contains files
    if (event.files && event.files.length > 0) {
      messageType = 'file'; // Set type to 'file' for uploaded files
      files = event.files.map((file: File) => ({
        url: URL.createObjectURL(file),
        type: file.type.split('/')[0], // Determine type (e.g., 'image', 'video')
        name: file.name,
        // icon: file.type.startsWith('image') ? false : 'file-text-outline', // Add icons for non-image files
        icon: 'file-text-outline', // Add icons for non-image files
      }));
      messageContent = 'Files uploaded'; // General message for file uploads
    }
  
    // Check if the event contains a quote
    if (event.quote) {
      messageType = 'quote';
      quote = event.quote;
      messageContent = event.message; // Message content alongside the quote
    }
  
    // Check if the event contains map coordinates
    if (event.latitude && event.longitude) {
      messageType = 'map';
      latitude = event.latitude;
      longitude = event.longitude;
      messageContent = 'Meet me here';
    }
  
    // Push the new message to the messages array
    this.messages.push({
      text: messageContent,
      reply: true, // User's message
      date: new Date(),
      type: messageType,
      files: files,
      quote: quote,
      latitude: latitude,
      longitude: longitude,
      user: {
        name: 'Mahmoud Khalifa',
        avatar: 'https://i.gifer.com/no.gif', // Replace with user's avatar URL
      },
    });
  
    // Sort messages by date to ensure correct order
    // this.sortMessages();

    const botReply = this.chatService.reply(messageContent);

  // Push the new message to the messages array
  this.messages.push(botReply); 

  // Sort messages by date to ensure correct order
  // this.sortMessages();

  }
  
  getMessagesName():string {
    return "Client direct messages";
  }

  // saveNewCommentToDatabase(event: any, isExternalChat:boolean){

  //   let newMessage = new TicketCommentListDto();
  //   newMessage.Comment = event.message;
  //   newMessage.IsReply = false;
  //   newMessage.IsExternalChat = isExternalChat;
  //   newMessage.Type = 'text';
  //   //change to current user id
  //   let userObj = this.authService.getUserInfoFromToken();

  //   if(userObj){
  //     newMessage.UserId = userObj.Id as number;
  //   }

  //   newMessage.TicketId = this.ticket.Id;
  //   let formData = convertToFormData(newMessage);

  //   this.ticketsCommmentsApiService.save(formData).subscribe((response : ApiResponse<TicketCommentListDto>) => {
  //     this.log.info(response);
  //      if(response.StatusCode === 200)
  //      {

  //       this.messages.push({
  //         text: event.message,
  //         date: new Date(),
  //         reply: false,
  //         // type: files.length ? 'file' : 'text',
  //         type: 'text',
  //         // files: files,
  //         user: {
  //           name: userObj.FirstName + " " + userObj.LastName,
  //           id: userObj.Id,
  //           // avatar: 'https://i.gifer.com/no.gif',
  //         },
  //       });

  //       this.toastNotificationService.showToast(NotitficationsDefaultValues.Success, 'Ticket comment', 'new comment has been saved');
  //      }
  //      else
  //      {
  //       this.toastNotificationService.showToast(NotitficationsDefaultValues.Danger, 'comment', response.ErrorMessage);
  //      }

  //       },
  //       (erorr) => {
  //       this.log.error(erorr);
  //       this.toastNotificationService.showToast(NotitficationsDefaultValues.Danger, 'comment', erorr);

  //     });
  // }

}