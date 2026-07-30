import {Component, signal} from '@angular/core';
import {RouterOutlet} from '@angular/router';

type ShareItem = {
  title: string;
  description: string;
  link: string;
};

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('LampShadeFront');
  protected readonly statusMessage = signal('');

  protected readonly shareItems: ShareItem[] = [
    {
      title: 'Explore the Docs',
      description: 'Open Angular docs in a new tab or share the link with someone else.',
      link: 'https://angular.dev'
    },
    {
      title: 'Learn with Tutorials',
      description: 'A good link to copy into chat, email, or notes.',
      link: 'https://angular.dev/tutorials'
    },
    {
      title: 'CLI Docs',
      description: 'Share the CLI guide when teammates need setup help.',
      link: 'https://angular.dev/tools/cli'
    }
  ];

  protected async shareItem(item: ShareItem): Promise<void> {
    if (navigator.share) {
      try {
        await navigator.share({
          title: item.title,
          text: item.description,
          url: item.link
        });
        this.statusMessage.set(`Shared “${item.title}”.`);
        return;
      } catch {
        // User cancelled the native share sheet, so keep the UI quiet.
        return;
      }
    }

    await this.copyLink(item);
    this.statusMessage.set(`Share is not supported here, so the link for “${item.title}” was copied instead.`);
  }

  protected async copyLink(item: ShareItem): Promise<void> {
    try {
      if (navigator.clipboard?.writeText) {
        await navigator.clipboard.writeText(item.link);
      } else {
        const textarea = document.createElement('textarea');
        textarea.value = item.link;
        textarea.setAttribute('readonly', 'true');
        textarea.style.position = 'fixed';
        textarea.style.opacity = '0';
        document.body.appendChild(textarea);
        textarea.select();
        document.execCommand('copy');
        document.body.removeChild(textarea);
      }

      this.statusMessage.set(`Copied link for “${item.title}”.`);
    } catch {
      this.statusMessage.set(`Could not copy the link for “${item.title}”.`);
    }
  }
}
