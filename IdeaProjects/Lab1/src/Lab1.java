import java.util.Scanner;
public class Lab1 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.println("Hello. What is your name?");
        String stringInput = scanner.next();

        System.out.println("It is nice to meet you " + stringInput + ". How old are you?");
        int integerInput = scanner.nextInt();

        System.out.println("I see that you are still quite young at only " + integerInput + ".");

        System.out.println("Where do you live?");

        String live = scanner.next();
        System.out.println("Wow! Ive always wanted to go to " + live + ". Thanks for chatting with me. Bye!");
    }

}
