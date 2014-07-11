import java.io.*;
import java.util.*;

public class Euro {
	public static void main(String[] args) {
HashMap<String, String> map = new HashMap<String, String>();
		BufferedReader reader = new BufferedReader(new FileReader(
				"accesories.txt"));
		String line = "";
		while ((line = reader.readLine()) != null) {
			String token[] = line.split(" ");
			map.put(token[0], token[1]);
		}
		reader.close();
		
		List<Map.Entry<String, String>> list = new ArrayList<Map.Entry<String, String>>(
				map.entrySet());
		
		Collections.sort(list, new Comparator<Map.Entry<String, String>>() {
			@Override
			public int compare(Map.Entry<String, String> a,
					Map.Entry<String, String> b) {
				return a.getValue().compareToIgnoreCase(b.getValue());
			}
		});
		
		System.out.println(map);
		
		//KDTree tree = new KDTree(1, 1); 
		
		System.out.println("KDTree loaded");
	}
}
