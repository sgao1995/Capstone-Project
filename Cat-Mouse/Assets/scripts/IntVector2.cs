[System.Serializable]

// a helper structure for integer tuples
public struct IntVector2 {
	public int x, z;
	
	// constructor
	public IntVector2 (int x, int z) {
		this.x = x;
		this.z = z;
	}
	
	// an operator for addition
	public static IntVector2 operator + (IntVector2 num1, IntVector2 num2) {
		num1.x += num2.x;
		num1.z += num2.z;
		return num1;
	}
	
	// operator for division
	public static IntVector2 operator / (IntVector2 num1, int num2) {
		num1.x /= num2;
		num1.z /= num2;
		return num1;
    }
}

